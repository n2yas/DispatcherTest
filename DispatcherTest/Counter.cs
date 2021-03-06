﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DispatcherTest
{
    class Counter : DispatcherObject
    {
        int _Count = 0;
        public int Count
        {
            get
            {
                VerifyAccess();
                return _Count;
            }
        }

        public void Add(int n)
        {
            VerifyAccess();
            _Count += n;
        }
    }
}
