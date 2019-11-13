using System.Reflection;

namespace gs
{
    public abstract class Settings
    {
        public virtual void CopyValuesFrom<T>(T other) where T : Settings
        {
            foreach (PropertyInfo prop_this in GetType().GetProperties())
            {
                if (prop_this.CanWrite)
                {
                    PropertyInfo prop_other = other.GetType().GetProperty(prop_this.Name);
                    if (prop_other != null)
                    {
                        prop_this.SetValue(this, prop_other.GetValue(other));
                    }
                }
            }

            foreach (FieldInfo field_this in GetType().GetFields())
            {
                FieldInfo field_other = other.GetType().GetField(field_this.Name);
                if (field_other != null)
                {
                    field_this.SetValue(this, field_other.GetValue(other));
                }
            }
        }

        public virtual T CloneAs<T>() where T : Settings, new()
        {
            var clone = new T();
            clone.CopyValuesFrom(this);
            return clone;
        }

        public string ClassTypeName
        {
            get { return GetType().ToString(); }
        }
    }

}