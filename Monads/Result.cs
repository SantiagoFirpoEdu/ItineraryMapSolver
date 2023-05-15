namespace ItineraryMapSolver.Monads;

public struct Result<TOkType, TErrorType>
{
	public static Result<TOkType, TErrorType> Ok(TOkType okValue)
	{
		Result<TOkType, TErrorType> result = new(okValue);
		return result;
	}

	public static  Result<TOkType, TErrorType> Error(TErrorType errorValue)
	{
		Result<TOkType, TErrorType> result = new(errorValue);
		return result;
	}

	public bool WasSuccessful()
	{
		return data.GetIsLeft();
	}

	public TOkType GetOkValueOr(TOkType defaultValue)
	{
		return data.GetLeftValueOr(defaultValue);
	}

	public TOkType GetOkValueUnsafe()
	{
		if (!WasSuccessful())
		{
			throw new MemberAccessException("Tried to access ok value when result was unsuccessful");
		}

		return data.GetLeft().GetValue();
	}

	public TErrorType GetErrorValueUnsafe()
	{
		if (WasSuccessful())
		{
			throw new MemberAccessException("Tried to access error value when result was successful");
		}

		return data.GetRight().GetValue();
	}

	public TErrorType GetErrorValueOr(TErrorType defaultValue)
	{
		return data.GetRightValueOr(defaultValue);
	}

	public void MatchOk(Action<TOkType> okFunctor)
	{
		if (WasSuccessful())
		{
			data.MatchLeft(okFunctor);
		}
	}

	public void MatchError(Action<TErrorType> errorFunctor)
	{
		if (!WasSuccessful())
		{
			data.MatchRight(errorFunctor);
		}
	}

	public void Match(Action<TOkType> okFunctor, Action<TErrorType> errorFunctor)
	{
		if (WasSuccessful())
		{
			data.MatchLeft(okFunctor);
		}
		else
		{
			data.MatchRight(errorFunctor);
		}
	}

	public  TOutType MapExpression<TOutType>(Func<TOkType, TOutType> okMapper, Func<TErrorType, TOutType> errorMapper)
	{
		return WasSuccessful() ? okMapper.Invoke(data.GetLeft().GetValue())
			: errorMapper.Invoke(data.GetRight().GetValue());
	}

	public Result<TOutOkType, TOutErrorType> Map<TOutOkType, TOutErrorType>(Func<TOkType, TOutOkType> okMapper, Func<TErrorType, TOutErrorType> errorMapper)
	{
		return WasSuccessful()
			? Result<TOutOkType, TOutErrorType>.Ok(okMapper.Invoke(data.GetLeftValue()))
			: Result<TOutOkType, TOutErrorType>.Error(errorMapper.Invoke(data.GetRightValue()));
	}

	public  Result<TOkType, TOutErrorType> MapErrorValue<TOutErrorType>(Func<TErrorType, TOutErrorType> errorMapper)
	{
		return WasSuccessful() ? Result<TOkType, TOutErrorType>.Ok(data.GetLeft().GetValue())
			: Result<TOkType, TOutErrorType>.Error(errorMapper.Invoke(data.GetRight().GetValue()));
	}

	public  Result<TOutOkType, TErrorType> MapOkValue<TOutOkType>(Func<TOkType, TOutOkType> okMapper)
	{
		return WasSuccessful() ? Result<TOutOkType, TErrorType>.Ok(okMapper.Invoke(data.GetLeft().GetValue()))
			: Result<TOutOkType, TErrorType>.Error(data.GetRight().GetValue());
	}

	public static Result<TOkType, TErrorType> AndThen(Result<Result<TOkType, TErrorType>, TErrorType> result)
	{
		if (!result.WasSuccessful())
		{
			return Error(result.GetErrorValueUnsafe());
		}

		var innerResult = result.GetOkValueUnsafe();

		return innerResult.WasSuccessful() ? Ok(innerResult.GetOkValueUnsafe()) : Error(innerResult.GetErrorValueUnsafe());

	}

	public ref Option<TOkType> GetOk()
	{
		return ref data.GetLeft();
	}

	public Option<TErrorType> GetError()
	{
		return data.GetRight();
	}
	
	private Result(TOkType okValue)
	{
		data = Either<TOkType, TErrorType>.OfLeftType(okValue);
	}
	
	private Result(TErrorType errorValue)
	{
		data = Either<TOkType, TErrorType>.OfRightType(errorValue);
	}

	private readonly Either<TOkType, TErrorType> data;
}