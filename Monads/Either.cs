namespace ItineraryMapSolver.Monads;

public readonly struct Either<TLeftType, TRightType>
{
    public static Either<TLeftType, TRightType> OfRightType(TRightType rightValue)
    {
        Either<TLeftType, TRightType> either = new(rightValue);
        return either;
    }

    public static Either<TLeftType, TRightType> OfLeftType(TLeftType leftValue)
    {
        Either<TLeftType, TRightType> either = new(leftValue);
        return either;
    }

    public bool GetIsLeft()
    {
        return isLeft;
    }

    public TLeftType GetLeftValue()
    {
        return leftValue.GetValue();
    }
    
    public TRightType GetRightValue()
    {
        return rightValue.GetValue();
    }

    public TLeftType GetLeftValueOr(TLeftType defaultValue)
    {
        return leftValue.GetValueOr(defaultValue);
    }

    public TRightType GetRightValueOr(TRightType defaultValue)
    {
        return rightValue.GetValueOr(defaultValue);
    }

    public void MatchLeft(Action<TLeftType> leftFunctor)
    {
        if (isLeft)
        {
            leftValue.MatchSome(leftFunctor);
        }
    }

    public void MatchRight(Action<TRightType> rightFunctor)
    {
        if (!isLeft)
        {
            rightValue.MatchSome(rightFunctor);
        }
    }

    public void Match(Action<TLeftType> okFunctor, Action<TRightType> errorFunctor)
    {
        if (isLeft)
        {
            leftValue.MatchSome(okFunctor);
        }
        else
        {
            rightValue.MatchSome(errorFunctor);
        }
    }

    public TOutType MapExpression<TOutType>(Func<TLeftType, TOutType> leftMapper, Func<TRightType, TOutType> rightMapper)
    {
        return isLeft ? leftMapper.Invoke(leftValue.GetValue())
            : rightMapper.Invoke(rightValue.GetValue());
    }

    public  Either<TOutLeftType, TOutRightType> Map<TOutLeftType, TOutRightType>(Func<TLeftType, TOutLeftType> okMapper, Func<TRightType, TOutRightType> errorMapper)
    {
        return isLeft ? Either<TOutLeftType, TOutRightType>.OfLeftType(okMapper.Invoke(leftValue.GetValue()))
            : Either<TOutLeftType, TOutRightType>.OfRightType(errorMapper.Invoke(rightValue.GetValue()));
    }

    public Either<TOutLeftValue, TRightType> MapLeftValue<TOutLeftValue>(Func<TLeftType, TOutLeftValue> leftMapper)
    {
        return isLeft ? Either<TOutLeftValue, TRightType>.OfLeftType(leftMapper.Invoke(leftValue.GetValue()))
            : Either<TOutLeftValue, TRightType>.OfRightType(rightValue.GetValue());
    }

    public readonly Option<TLeftType> GetLeft()
    {
        return leftValue;
    }

    public readonly Option<TRightType> GetRight()
    {
        return rightValue;
    }

    public bool TryGetLeftValue(out TLeftType? outLeftValue)
    {
        return GetLeft().TryGetValue(out outLeftValue);
    }
    
    public bool TryGetRightValue(out TRightType? outRightValue)
    {
        return GetRight().TryGetValue(out outRightValue);
    }
    
    private Either(TLeftType leftValue)
    {
        this.leftValue = Option<TLeftType>.Some(leftValue);
        rightValue = Option<TRightType>.None();
        isLeft = true;
    }
    
    private Either(TRightType rightValue)
    {
        this.rightValue = Option<TRightType>.Some(rightValue);
        leftValue = Option<TLeftType>.None();
        isLeft = false;
    }

    private readonly Option<TLeftType> leftValue;
    private readonly Option<TRightType> rightValue;
    private readonly bool isLeft;

}