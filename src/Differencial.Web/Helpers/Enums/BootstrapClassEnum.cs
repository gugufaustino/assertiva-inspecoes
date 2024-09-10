using Differencial.Domain;
using Differencial.Domain.Annotation;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Enums.WorkFlow;
using Differencial.Web.Generico;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Differencial.Web.Helpers
{ 
    public enum TamanhoEnum
    {
        ModalPequena = 0,
        ModalPadrao = 1,
        ModalLarga = 2
    }

    public enum BtnCorEnum
    {
        Default = 0,
        primary = 1,
        success = 2,
        white = 3
    }

    public enum BtnTamanhoEnum
    {
        Pequena = 0,
        Padrao = 1,
        Grande = 2
    }

    public enum IconeEnum
    { 
        SemIcone = 0,
        pencil = 1,
        check = 2,
        floppySave = 3
    }

}