namespace ItineraryMapSolver.Model;

public class ReadonlyRef<TReferencedType> where TReferencedType : struct
{
    public ReadonlyRef(in TReferencedType referencedStruct)
    {
        _value = referencedStruct;
    }

    public ref readonly TReferencedType GetValueRef()
    {
        return ref _value;
    }

    private readonly TReferencedType _value;
}

