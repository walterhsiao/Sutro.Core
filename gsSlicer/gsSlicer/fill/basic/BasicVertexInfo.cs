namespace gs
{
    public class BasicVertexInfo
    {
        public BasicVertexInfo()
        {

        }

        public BasicVertexInfo(BasicVertexInfo other)
        {
        }

        public virtual BasicVertexInfo Interpolate(BasicVertexInfo other, double param)
        {
            return new BasicVertexInfo();
        }
    }
}