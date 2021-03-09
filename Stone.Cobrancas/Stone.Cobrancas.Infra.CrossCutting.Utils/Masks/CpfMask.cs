using Stone.Cobrancas.Infra.CrossCutting.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Stone.Cobrancas.Infra.CrossCutting.Utils.Masks
{
    public sealed class CpfMask : ICpfMask
    {
        public string RemoveMaskCpf([NotNull] in string cpf)
        {
            return cpf.Replace(".", string.Empty).Replace("-", string.Empty);
        }
    }
}
