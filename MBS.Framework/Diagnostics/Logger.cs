//
//  Logger.cs
//
//  Author:
//       beckermj <>
//
//  Copyright (c) 2023 ${CopyrightHolder}
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;

namespace MBS.Framework.Diagnostics
{
	public class Logger
	{
		public void Write(string value)
		{
			Console.Error.Write(value);
		}
		public void WriteLine(string value)
		{
			Console.Error.WriteLine(value);
		}

		public string ProjectName { get; set; }

		private class FuncInfo
		{
			public string FileName;
			public string FunctionName;

			public FuncInfo(string filename, string functionName)
			{
				FileName = filename;
				FunctionName = functionName;
			}
		}

		private Stack<FuncInfo> _funcInfo = new Stack<FuncInfo>();
		public void EnterFunction(string filename, string funcName)
		{
			Console.Error.WriteLine("entering function {0}:{1}", filename, funcName);
			_funcInfo.Push(new FuncInfo(filename, funcName));
		}
		public void LeaveFunction()
		{
			FuncInfo f = _funcInfo.Pop();
			Console.Error.WriteLine("leaving function {0}:{1}", f.FileName, f.FunctionName);
		}

		private FuncInfo CurrentFunction
		{
			get
			{
				if (_funcInfo.Count > 0)
				{
					return _funcInfo.Peek();
				}
				return null;
			}
		}

		private void WriteMessage(string filename, int lineNumber, MessageSeverity severity, string funcName, string message)
		{
			WriteMessage(filename, lineNumber, ProjectName, severity, funcName, message);
		}
		private void WriteMessage(string filename, int lineNumber, string projName, MessageSeverity severity, string funcName, string message)
		{
			string severityStr = null;
			switch (severity)
			{
				case MessageSeverity.Error: severityStr = "CRITICAL"; break;
				case MessageSeverity.Message: severityStr = "MESSAGE"; break;
				case MessageSeverity.Warning: severityStr = "WARNING"; break;
				case MessageSeverity.None: severityStr = null; break;
			}
			if (severityStr == null)
			{
				WriteLine(String.Format("({0}:{1}): {2}: {3}: {4}: {5}", filename, lineNumber, projName, DateTime.Now.ToString(), funcName, message));
			}
			else
			{
				WriteLine(String.Format("({0}:{1}): {2}[{3}]: {4}: {5}: {6}", filename, lineNumber, projName, severityStr, DateTime.Now.ToLongTimeString(), funcName, message));
			}
		}
		public void WriteAssertFailed(string filename, int lineNumber, string projName, MessageSeverity severity, string funcName, string call, string parms)
		{
			WriteMessage(filename, lineNumber, projName, severity, funcName, String.Format("assertion '{0} ({1})' failed", call, parms));
		}

		public void WriteMismatch(int lineNumber, MessageSeverity severity, string message, object expectedValue, object actualValue)
		{
			string filename = CurrentFunction?.FileName;
			string funcName = CurrentFunction?.FunctionName;
			WriteMessage(filename, lineNumber, severity, funcName, String.Format("{0}: expected {1}, got {2}", message, expectedValue, actualValue));
		}
	}
}
