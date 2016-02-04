﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBarLabs.Web
{
    public class ResourceCantBeUpdatedException : ResourceConflictException
    {
        public ResourceCantBeUpdatedException()
        {
        }

        public ResourceCantBeUpdatedException(string message) : base(message)
        {
        }
    }
}
