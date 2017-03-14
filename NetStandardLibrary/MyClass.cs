namespace NetStandardLibrary
{
    public class MyClass : IMyInterface
    {
        public MyClass(int i) => MyProperty = i;

        public int MyProperty { get; set; }

        public string MyMethod()
        {
            return GetType().FullName;
        }
    }
}
