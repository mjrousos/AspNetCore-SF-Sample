namespace NetStandardLibrary
{
    public class MyClass : IMyInterface
    {
        public MyClass(int i) => MyProperty = i;

        private int myPrivateField;
        public int MyProperty { get => myPrivateField++; set => myPrivateField = value; }

        public string MyMethod()
        {
            return GetType().FullName;
        }
    }
}
