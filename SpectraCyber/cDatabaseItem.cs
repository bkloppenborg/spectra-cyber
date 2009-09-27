using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCyber
{
    enum eDBCommandType
    {
        Data,
        Settings,
        ScanStart,
        ScanStop,
        Terminate
    }

    class cDatabaseItem
    {
        private string[] mstrarrFields;
        private object[] moarrValues;
        private eDBCommandType meType;

        public cDatabaseItem(eDBCommandType eType)
        {
            mstrarrFields = new string[0];
            moarrValues = new object[0];
            meType = eType;
        }

        public cDatabaseItem(string strField, object oValue, eDBCommandType eType)
        {
            mstrarrFields = new string[] { strField };
            moarrValues = new object[] { oValue };
            meType = eType;
        }

        public cDatabaseItem(string[] arrField, object[] arrValue, eDBCommandType eType)
        {
            mstrarrFields = arrField;
            moarrValues = arrValue;
            meType = eType;
        }

        public eDBCommandType Type
        {
            get
            {
                return meType;
            }
        }

        // Get the fields as a string of comma-seperated field (that is "field, field, ... , field")
        public string Fields
        {
            get
            {
                return (string.Join(", ", mstrarrFields));
            }
        }

        // Get the values as a string of comma-seperated field (that is "field, field, ... , field")
        public string Values
        {
            get
            {
                string strTemp;
                string[] arrValues = new string[moarrValues.Length];

                // Loop through the fields.  Any strings need to have any quotes escaped.
                for(int i = 0; i < moarrValues.Length; i++)
                {
                    if (moarrValues[i] is string)
                    {
                        strTemp = (string)moarrValues[i];
                        arrValues[i] = "'" + strTemp.Replace("'", "''") + "'";
                    }
                    else if (moarrValues[i] == null)
                        arrValues[i] = "null";
                    else
                        arrValues[i] = moarrValues[i].ToString();
                }

                return (string.Join(", ", arrValues));
            }
        }
    }
}
