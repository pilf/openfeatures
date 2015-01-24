using System;

namespace OpenTable.Features.Core
{
    public class Either<L, R> 
		where L: class
		where R: class
	{
		private readonly L _left;
		private readonly R _right;

		public Either(L left, R right)
		{
			_left = left;
			_right = right;
		}

		public T Do<T>(Func<L, T> leftFunc, Func<R, T> rightFunc)
		{
		    if(_left != null)
				return leftFunc(_left);
		    return rightFunc(_right);
		}

	    public void Do(Action<L> leftAction, Action<R> rightAction)
		{
			if(_left != null)
				leftAction(_left);
			else
				rightAction(_right);
		}
	}
}