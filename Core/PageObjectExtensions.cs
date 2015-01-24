using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.Core
{
	public static class PageObjectExtensions
	{
		public static T AssertPageIsNow<T>(this IPageObject page) where T : IPageObject
		{
			return AssertPageIsNow<T>(new[] { page });
		}

		public static T AssertPageIsNow<T>(this IEnumerable<IPageObject> pages) where T : IPageObject
		{
			var page = pages.OfType<T>().SingleOrDefault();

			if (page == null)
				throw new Exception("The page type given was not of the expected type");

			var i = 0;

			while (i <= 35 && !page.IsCurrentPage())
			{
				Thread.Sleep(500);
				i++;
			}

			if (!page.IsCurrentPage())
				throw new Exception("Expected the page object to be current page, but wasn't");

			return page;
		}
	}
}