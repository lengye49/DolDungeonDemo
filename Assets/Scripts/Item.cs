
public class Item  {
    public int id;
    public string name;
    public int price;
    public string desc;
    public string imageName;
}

public class Gift{
    public int id;
    public string name;
    public int family;
    public string desc;
    public int level;
    public int type;
}

public class DgEvent{
    public int id;
    public string name;
    public string desc;
    public string opt1;
    public string opt1Desc;
    public string opt2;
    public string opt2Desc;
    public string opt3;
    public string opt3Desc;
}

public class ShopItem{
    public int itemId;
    public bool isSellOut;
}

public class Shop{
    public ShopItem[] shopItems;
}
    