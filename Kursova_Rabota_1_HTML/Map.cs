using System;
using System.Collections.Generic;
using System.Text;

namespace Kursova_Rabota_1_HTML
{
    public class KeyValuePair
    {
        public string key;
        public string value;

        public KeyValuePair(string key_, string value_)
        {
            key = key_;
            value = value_;
        }
    }
    public class Map
    {
        private MyList<KeyValuePair> _data;

        public Map()
        {
            _data = new MyList<KeyValuePair>();
        }

        public void Add(string key, string value)
        {
            bool inMap = false;
            foreach (KeyValuePair item in _data)
            {
                if(item.key == key)
                {
                    item.value = value;
                    inMap = true;
                }
            }

            if(inMap == false)
            {
                _data.Add(new KeyValuePair(key, value));
            }
        }

        public string Get(string key)
        {
            foreach (KeyValuePair item in _data)
            {
                if (item.key == key)
                {
                    return item.value;
                }
            }

            return "";
        }

        public bool ContainsSameData(Map other)
        {
            bool equels = false;
            foreach (KeyValuePair item in _data)
            {
                equels = false;
                foreach (KeyValuePair otherItem in other._data)
                {
                    if(item.key == otherItem.key)
                    {
                        if (item.value == otherItem.value)
                        {
                            equels = true;
                            break;
                        }
                    }
                }
                if (equels == false)
                    return false;
            }

            return true;
        }
    }
}
