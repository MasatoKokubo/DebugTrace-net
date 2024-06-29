// Entities.cs
// (C) 2018 Masato Kokubo
public class FieldsProperties {
    private            int           PrivateField = 1;
    protected          int         ProtectedField = 2;
    internal           int          InternalField = 3;
    protected internal int ProtectedInternalField = 4;
    private protected  int  PrivateProtectedField = 5;
    public             int            PublicField = 6;

    private            int           PrivateProperty {get;} = 1;
    protected          int         ProtectedProperty {get;} = 2;
    internal           int          InternalProperty {get;} = 3;
    protected internal int ProtectedInternalProperty {get;} = 4;
    private protected  int  PrivateProtectedProperty {get;} = 5;
    public             int            PublicProperty {get;} = 6;
}
