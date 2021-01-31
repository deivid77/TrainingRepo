using System;
using Core.Common.Core;
using FluentValidation;

namespace Core.Common.Tests
{
    internal class TestClass : ObjectBase
    {
        string _cleanProp = string.Empty;
        string _dirtyProp = string.Empty;
        string _stringProp = string.Empty;
        TestChild _child = new TestChild();
        TestChild _notNavigableChild = new TestChild();

        public string CleanProp
        {
            get { return _cleanProp; }
            set
            {
                if (_cleanProp == value)
                    return;

                _cleanProp = value;
                OnPropertyChanged(false);
            }
        }

        public string DirtyProp
        {
            get { return _dirtyProp; }
            set
            {
                if (_dirtyProp == value)
                    return;

                _dirtyProp = value;
                OnPropertyChanged();
            }
        }

        public string StringProp
        {
            get { return _stringProp; }
            set
            {
                if (_stringProp == value)
                    return;

                _stringProp = value;
                OnPropertyChanged(false, "StringProp");
            }
        }

        public TestChild Child
        {
            get { return _child; }
        }

        [NotNavigable]
        public TestChild NotNavigableChild
        {
            get { return _notNavigableChild; }
        }

        class TestClassValidator : AbstractValidator<TestClass>
        {
            public TestClassValidator()
            {
                RuleFor(obj => obj.StringProp).NotEmpty();
            }
        }

        protected override IValidator GetValidator()
        {
            return new TestClassValidator();
        }
    }
}
