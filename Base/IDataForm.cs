using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Base
{
    public interface IDataForm
    {
        /// <summary>
        /// This method must be used to ensure that the data is valid or at least if there's any.
        /// </summary>
        /// <returns>True if data is valid</returns>
        bool ValidateFields();

        /// <summary>
        /// This method must be used to load data into the form
        /// </summary>
        void LoadData();
    }
}