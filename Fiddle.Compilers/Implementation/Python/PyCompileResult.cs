﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Fiddle.Compilers.Implementation.Python
{
    public class PyCompileResult : ICompileResult
    {
        public long Time { get; }
        public bool Success { get; }
        public string SourceCode { get; }
        public IEnumerable<IDiagnostic> Diagnostics { get; }
        public IEnumerable<IDiagnostic> Warnings { get; }
        public IEnumerable<Exception> Errors { get; }


        public PyCompileResult(long time, string code,
            IEnumerable<IDiagnostic> diagnostics)
        {
            Time = time;
            SourceCode = code;


            IEnumerable<IDiagnostic> enumerable = diagnostics as IDiagnostic[] ?? diagnostics.ToArray();
            Diagnostics = enumerable;
            Warnings = enumerable.Where(d => d.Severity == Microsoft.Scripting.Severity.Warning);
            Errors = enumerable
                .Where(d => d.Severity == Microsoft.Scripting.Severity.Error)
                .Select(dd => new Exception($"Ln{dd.Line} Ch{dd.Char}: {dd.Message}"));

            if (!Errors.Any())
                Success = true;
        }
    }
}