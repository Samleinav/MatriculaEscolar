using System;

namespace ModelsBaseApp
{
    public class MethodResponse<T>
    {
        public string[] MSG { get; set; }
        public bool result { get; set; }
        public string type { get; set; }
        public bool exit { get; set; }
        public T Data { get; set; }

        public static implicit operator T(MethodResponse<T> value)
        {
            return value.Data;
        }

        public static implicit operator MethodResponse<T>(T value)
        {
            return new MethodResponse<T> { Data = value };
        }
    }

    public class MethodResponse
    {
        public string[] MSG { get; set; }
        public bool result { get; set; }
        public string type { get; set; }
        public bool exit { get; set; }
    }
}
