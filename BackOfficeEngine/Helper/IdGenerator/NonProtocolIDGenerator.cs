﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using BackOfficeEngine.AppConstants;

namespace BackOfficeEngine.Helper.IdGenerator
{
    internal class NonProtocolIDGenerator : BaseIDGenerator<NonProtocolIDGenerator>
    {

        protected override object fileLock { get; set; }
        protected override string filePath { get; set; }
        protected override object counterLock { get; set; }

        /// <summary>
        /// do not call this constructor directly. Use GetInstanceMethod to get the instance of generator
        /// </summary>
        public NonProtocolIDGenerator()
        {
            fileLock = new object();
            filePath = CommonFolders.NonProtocolOrderIDPath;
            counterLock = new object();
        }
    }
}
