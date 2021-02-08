namespace ForgeLibs.Models.Forge
{
	//public class ForgePropertySelector
	//{
	//    private string _name = "";
	//    public string Name
	//    {
	//        get { return _name; }
	//        set { _name = value; }
	//    }

	//    private string _value;

	//    public string Value
	//    {
	//        get { return _value; }
	//        set
	//        {
	//            _value = value;
	//            OnValueChanged(new ForgePropertyChangedEventArgs(_value));
	//        }
	//    }

	//    public ForgePropertySelector(string name)
	//    {
	//        _name = name;
	//    }

	//    public static IEnumerable<ForgePropertySelector> SetProperties(int count)
	//    {
	//        for (int i = 0; i < count; i++)
	//        {
	//            yield return new ForgePropertySelector("");
	//        }
	//    }

	//    //public EventCallback<string> OnValueChanged2 { get; set; }
	//    public event EventHandler<ForgePropertyChangedEventArgs> ValueChanged;

	//    public void OnValueChanged(ForgePropertyChangedEventArgs args)
	//    {
	//        EventHandler<ForgePropertyChangedEventArgs> handler = ValueChanged;
	//        if (handler != null)
	//        {
	//            handler(this, args);
	//        }
	//    }
	//}

	//public class ForgePropertyChangedEventArgs : EventArgs
	//{
	//    public string Value { get; set; }
	//    public ForgePropertyChangedEventArgs(string str)
	//    {
	//        Value = str;
	//    }
	//}
}
