using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPSP.Model.Objects
{
    public class Money
    {
        private Decimal _moneyValue;

        private bool _isNull;


        //Properties
        public string MoneyValue
        {
            get
            {
                if (!_isNull)//Note: This uses a rounding method. It round up or down depending on the previous decimal value
                    return Math.Round(_moneyValue,2,MidpointRounding.AwayFromZero).ToString("N");
                    //return _moneyValue.ToString("N");
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    if (Utilities.IsNumeric(value.ToString()))
                    {
                        _moneyValue = Convert.ToDecimal(value);
                        _isNull = false;
                    }
                    else
                        _isNull = true;
                }
                else
                    _isNull = true;
            }
        }

        public bool IsNull
        {
            get
            {
                return _isNull;
            }
            set
            {
                _isNull = value;
            }
        }

        //Constructers
        public Money()
        {//We dont have a value. So we gotta tell em we are null. Null may not be fashionable but its alls we gots.
            _isNull = true;
        }

        public Money(string strMoney)
        {//We dont know what that crazy strMoney could be so lets use our property to check on it.
            MoneyValue = strMoney;
        }

        public Money(Decimal valMoney)
        {//We gots a decimal so no worries if its numeric or any of that jazz
            _moneyValue = valMoney;
            _isNull = false;
        }

        //Methods
        public void addValue(string addValue)
        {
            if (Model.Utilities.IsNumeric(addValue))
                this.addValue(Convert.ToDecimal(addValue));
        }
        public void addValue(decimal addValue)
        {
            if (!_isNull)
                _moneyValue += addValue;
            else
                MoneyValue = addValue.ToString();
        }

        public decimal getDecimalValue()
        {//Its for when you want 0 instead of those pesky nulls
            if (!_isNull)
                return _moneyValue;
            else
                return 0;
        }

        public decimal getRoundedDecimalValue()
        {//Its for when you want 0 instead of those pesky nulls
            if (!_isNull)
                return Math.Round(_moneyValue, 2, MidpointRounding.AwayFromZero);
            else
                return 0;
        }

        //Overrides
        public override string ToString()
        {
            if (!_isNull)
            {
                if (_moneyValue != 0)
                {
                    return String.Format("{0:C}", _moneyValue);
                    //return "$" + _moneyValue.ToString("#.##");
                }
                else
                    return "$0.00";
            }
            else
                return MoneyValue;
        }

    }
}