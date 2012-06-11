/* 
 * Copyright (c) 2007 Brian Kloppenborg
 *
 * If you use this software as part of a scientific publication, please cite as:
 *
 * Kloppenborg, B. (2012), "SpectraCyber Control Software"
 * (Version X). Available from  <https://github.com/bkloppenborg/spectra-cyber>.
 *
 * This file is part of the SpectraCyber Control Software (SCCS).  
 *
 * The SpectraCyber was developed by Dr. John Bernard and gifted to
 * Radio Astronomy Supplies (RAS).  "SpectraCyber" is used with 
 * permission from RAS.
 * 
 * SCCS is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License 
 * as published by the Free Software Foundation version 3.
 * 
 * SCCS is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public 
 * License along with SCCS.  If not, see <http://www.gnu.org/licenses/>.
 */

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
