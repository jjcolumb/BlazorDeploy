using System;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace BlazorLinux.Module
{
    [DefaultClassOptions]
    public class Customer : BaseObject
    {
        public Customer() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Customer(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        string _name;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }

        string _address;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Address
        {
            get => _address;
            set => SetPropertyValue(nameof(Address), ref _address, value);
        }

        bool _active;
        public bool Active
        {
            get => _active;
            set => SetPropertyValue(nameof(Active), ref _active, value);
        }
    }

}