namespace ItineraryMapSolver.Monads;

public readonly struct Option<TSomeValueType>
{
	public Option()
	{
		isSet = false;
		value = default;
	}

	public static Option<TSomeValueType> Some(TSomeValueType value)
	{
		return new Option<TSomeValueType>(value);
	}

	public static Option<TSomeValueType> None()
	{
		return new Option<TSomeValueType>();
	}

	public Option<TReturnType> Map<TReturnType>(Func<TSomeValueType, TReturnType> optionMapper)
	{
		return isSet ? new Option<TReturnType>(optionMapper.Invoke(value!))
			: new Option<TReturnType>();
	}

	public TOutType MapExpression<TOutType>(Func<TOutType> someMapper, Func<TOutType> noneMapper)
	{
		return isSet ? someMapper.Invoke()
			: noneMapper.Invoke();
	}

	public TOutType MapExpression<TOutType>(Func<TSomeValueType, TOutType> someMapper, Func<TOutType> noneMapper)
	{
		return isSet ? someMapper.Invoke(value!)
			: noneMapper.Invoke();
	}

	public Option<TOutOptionalType> AndThen<TOutOptionalType>(Func<TSomeValueType, Option<TOutOptionalType>> optionMapper)
	{
		var nestedOption = Map(optionMapper);

		return nestedOption.IsSet() && (nestedOption.GetValue().IsSet()) ? Option<TOutOptionalType>.Some(nestedOption.GetValue().GetValue())
			: Option<TOutOptionalType>.None();
	}

	public void MatchSome(Action<TSomeValueType> someFunctor)
	{
		if (IsSet())
		{
			someFunctor.Invoke(value!);
		}
	}

	public void MatchNone(Action noneFunctor)
	{
		if (!IsSet())
		{
			noneFunctor.Invoke();
		}
	}

	public void Match(Action<TSomeValueType> someFunctor, Action noneFunctor)
	{
		if (someFunctor == null)
		{
			throw new ArgumentNullException(nameof(someFunctor));
		}

		if (IsSet())
		{
			someFunctor.Invoke(value!);
		}
		else
		{
			noneFunctor.Invoke();
		}
	}

	public TSomeValueType GetValue()
	{
		if (!isSet)
		{
			throw new NullReferenceException("Tried to access a Option's value while it was empty");
		}
		return value!;
	}
	
	public bool TryGetValue(out TSomeValueType? outValue)
	{
		if (isSet)
		{
			outValue = value;
			return true;
		}

		outValue = default;
		return false;
	}

	public bool IsSet()
	{
		return isSet;
	}

	public bool IsEmpty()
	{
		return !IsSet();
	}

	public TSomeValueType GetValueOr(TSomeValueType defaultValue)
	{
		return  (isSet ? value : defaultValue)!;
	}

	private Option(TSomeValueType value)
	{
		isSet = true;
		this.value = value;
	}

	private readonly bool isSet;
	private readonly TSomeValueType? value;
}