using System;

public class Class1
{
	public Class1()
	{
	}
    /*
    private List<Object> GetComponents(Object obj)
    {
        List<Object> components = new List<Object>();
        var bindingFlags = BindingFlags.Instance |
                           BindingFlags.NonPublic |
                           BindingFlags.Public;

        FieldInfo[] fi = obj.GetType().GetFields(bindingFlags);
        foreach (FieldInfo f in fi)
        {
            String name = f.Name;
            Object value = f.GetValue(obj);

            components.Add(f.Name);
            if (f.FieldType.IsPrimitive || f.FieldType == typeof(String) || f.FieldType == typeof(Decimal))
            {
                components.Add(f.GetValue(obj));
            }
            else if (f.GetValue(obj) is IList)
            {
                IList list1 = (IList)f.GetValue(obj);
                for (int i = 0; i < list1.Count; i++)
                {
                    components.Add(GetComponents(f.GetValue(obj)));

                }
            }
            else
            {
                components.Add(GetComponents(f.GetValue(obj)));
            }
        }

        return components;
    }

    private List<Object> GetChanges(Object obj1, Object obj2)
    {

        var varriance = new List<Object>();

        var bindingFlags = BindingFlags.Instance |
                           BindingFlags.NonPublic |
                           BindingFlags.Public;

        FieldInfo[] fi = obj1.GetType().GetFields(bindingFlags);
        foreach (FieldInfo f in fi)
        {
            String name = f.Name;
            Object valueActualWorld = f.GetValue(obj1);
            Object valuePastWorld = f.GetValue(obj2);

            if (!Equals(valueActualWorld, valuePastWorld))
            {
                varriance.Add(f.Name);
                if (f.FieldType.IsPrimitive || f.FieldType == typeof(String) || f.FieldType == typeof(Decimal))
                {
                    varriance.Add(f.GetValue(obj1));
                }
                else if (f.GetValue(obj1) is IList)
                {
                    IList list1 = (IList)f.GetValue(obj1);
                    IList list2 = (IList)f.GetValue(obj2);
                    for (int i = 0; i < Math.Max(list1.Count, list2.Count); i++)
                    {
                        if (i < Math.Min(list1.Count, list2.Count))
                        {
                            varriance.Add(GetChanges(list1[i], list2[i]));
                        }
                        else
                        {
                            if (list1.Count > list2.Count)
                            {
                                varriance.Add(GetComponents(list1[i]));
                            }
                            if (list1.Count < list2.Count)
                            {
                                varriance.Add(GetComponents(list2[i]));
                            }
                        }
                    }
                }
                else
                {
                    varriance.Add(GetChanges(f.GetValue(obj1), f.GetValue(obj2)));
                }
            }
        }

        return varriance;
    }
    // {"Height", 480, "Width", 720, "PlayerOne",{"X", 40, "Y", 80}, 

    private void PrintChanges(List<Object> changes)
    {
        foreach (Object change in changes)
        {
            if (change.GetType().IsPrimitive || change.GetType() == typeof(String) || change.GetType() == typeof(Decimal))
            {
                Console.WriteLine(change);
            }
            else
            {
                PrintChanges((List<Object>)change);
            }
        }
    }
    */
}
