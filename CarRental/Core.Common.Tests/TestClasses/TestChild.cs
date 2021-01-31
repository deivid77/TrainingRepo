using System;
using System.Collections.Generic;
using Core.Common.Core;

namespace Core.Common.Tests
{
    internal class TestChild : ObjectBase
    {
        string _childName = string.Empty;

        public string ChildName
        {
            get { return _childName; }
            set
            {
                if (_childName == value)
                    return;

                _childName = value;
                OnPropertyChanged();
            }
        }
    }
}
