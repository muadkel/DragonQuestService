using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPSP.Model.TypeConversions
{
    public class SafeInt
    {
        private int _intValue;
        private bool _isNull;




        public bool IsNull
        {
            get { return _isNull; }
            set { _isNull = value; }
        }


        public string IntValue
        {
            get
            {
                if (!_isNull)
                    return _intValue.ToString();
                else
                    return "";
            }
            set
            {
                if (Utilities.IsNumeric(value.ToString()))
                {
                    _intValue = Convert.ToInt32(value);
                    _isNull = false;
                }
                else
                {
                    // _intValue = 0;
                    _isNull = true;
                }
            }
        }

        public int intVal
        {
            get
            {
                if (IsNull)
                    return -1;
                else
                    return _intValue;
            }
        }


        public SafeInt()
        {
            _intValue = 0;
            _isNull = true;
        }

        public SafeInt(string strInt)
        {
            this.IntValue = strInt;
        }

        //Methods


        //Overrides
        public override string ToString()
        {
            if (!_isNull)
                return _intValue.ToString();
            else
                return "";
        }
    }
}