﻿using System;

namespace MoreNote.Common.entity
{
	public class FunResult
	{
		public bool OK = false;
		public String Message = "";
		public FunResult(bool ok, string ms)
		{
			OK = ok;
			Message = ms;
		}

	}
}
