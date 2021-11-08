﻿using System;

namespace Compentio.Example.DotNetCore.App.Entities
{
    public class NoteDao
    {
        public long Id { get; set; }
        public string PageTitle { get; set; }
        public string Description { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public NoteDocumentDao Document { get; set; }
    }
}