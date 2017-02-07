﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace g3
{
    public class MeshFaceSelection
    {
        public DMesh3 Mesh;

        HashSet<int> Selected;
        List<int> temp;

        public MeshFaceSelection(DMesh3 mesh)
        {
            Mesh = mesh;
            Selected = new HashSet<int>();
            temp = new List<int>();
        }


        private bool is_selected(int tid)
        {
            return Selected.Contains(tid);
        }
        private void add(int tid)
        {
            Selected.Add(tid);
        }
        private void remove(int tid)
        {
            Selected.Remove(tid);
        }



        public void Select(int tid)
        {
            Debug.Assert(Mesh.IsTriangle(tid));
            if (Mesh.IsTriangle(tid))
                add(tid);
        }
        public void Select(int[] triangles)
        {
            for ( int i = 0; i < triangles.Length; ++i ) {
                if (Mesh.IsTriangle(triangles[i]))
                    add(triangles[i]);
            }
        }




        public int[] ToArray()
        {
            int nTris = Selected.Count;
            int[] tris = new int[nTris];
            int i = 0;
            foreach (int tid in Selected)
                tris[i++] = tid;
            return tris;
        }



        public void ExpandToFaceNeighbours()
        {
            temp.Clear();

            foreach ( int tid in Selected ) { 
                Index3i nbr_tris = Mesh.GetTriNeighbourTris(tid);
                for (int j = 0; j < 3; ++j) {
                    if (nbr_tris[j] != DMesh3.InvalidID && is_selected(nbr_tris[j]) == false)
                        temp.Add(nbr_tris[j]);
                }
            }

            for (int i = 0; i < temp.Count; ++i)
                add(temp[i]);
        }


        public void ExpandToOneRingNeighbours()
        {
            temp.Clear();

            foreach ( int tid in Selected ) { 
                Index3i tri_v = Mesh.GetTriangle(tid);
                for (int j = 0; j < 3; ++j) {
                    int vid = tri_v[j];
                    foreach (int nbr_t in Mesh.VtxTrianglesItr(vid)) {
                        if (is_selected(nbr_t) == false)
                            temp.Add(nbr_t);
                    }
                }
            }

            for (int i = 0; i < temp.Count; ++i)
                add(temp[i]);
        }



        // return true if we clipped something
        public bool ClipFins()
        {
            temp.Clear();
            foreach (int tid in Selected) {
                if (is_fin(tid))
                    temp.Add(tid);
            }
            if (temp.Count == 0)
                return false;
            foreach (int tid in temp)
                remove(tid);
            return true;
        }


        // return true if we filled any ears.
        public bool FillEars()
        {
            // [TODO] not efficient! checks each nbr 3 times !! ugh!!
            temp.Clear();
            foreach (int tid in Selected) {
                Index3i nbr_tris = Mesh.GetTriNeighbourTris(tid);
                for (int j = 0; j < 3; ++j) {
                    int nbr_t = nbr_tris[j];
                    if (is_selected(nbr_t))
                        continue;
                    if (is_ear(nbr_t))
                        temp.Add(nbr_t);
                }
            }
            if (temp.Count == 0)
                return false;
            foreach (int tid in temp)
                add(tid);
            return true;
        }

        // returns true if selection was modified
        public bool LocalOptimize(bool bClipFins, bool bFillEars)
        {
            bool bModified = false;
            bool done = false;
            while ( ! done ) {
                done = true;
                if (bClipFins && ClipFins())
                    done = false;
                if (bFillEars && FillEars())
                    done = false;
                if (done == false)
                    bModified = true;
            }
            return bModified;
        }








        private void count_nbrs(int tid, out int nbr_in, out int nbr_out, out int bdry_e)
        {
            Index3i nbr_tris = Mesh.GetTriNeighbourTris(tid);
            nbr_in = 0; nbr_out = 0; bdry_e = 0;
            for ( int j = 0; j < 3; ++j ) {
                int nbr_t = nbr_tris[j];
                if (nbr_t == DMesh3.InvalidID)
                    bdry_e++;
                else if (is_selected(nbr_t) == true)
                    nbr_in++;
                else
                    nbr_out++;
            }
        }
        private bool is_ear(int tid)
        {
            if (is_selected(tid) == true)
                return false;
            int nbr_in, nbr_out, bdry_e;
            count_nbrs(tid, out nbr_in, out nbr_out, out bdry_e);
            if (bdry_e == 2 && nbr_in == 1) {
                return true;        // unselected w/ 2 boundary edges, nbr is  in
            } else if (nbr_in == 2) {
                if (bdry_e == 1 || nbr_out == 1)
                    return true;        // unselected w/ 2 selected nbrs
            }
            return false;
        }
        private bool is_fin(int tid)
        {
            if (is_selected(tid) == false)
                return false;
            int nbr_in, nbr_out, bdry_e;
            count_nbrs(tid, out nbr_in, out nbr_out, out bdry_e);
            return (nbr_in == 1 && nbr_out == 2);
        }

    }
}
