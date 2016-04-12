﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Workflow;
using iTextSharp.text.pdf;
using System.IO;
using System.ComponentModel.Composition;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;

namespace org.secc.PDF
{
    [ActionCategory( "PDF" )]
    [Description( "Creates pdf from lava" )]
    [Export( typeof( Rock.Workflow.ActionComponent ) )]
    [ExportMetadata( "ComponentName", "Lava PDF" )]
    class LavaPDF : ActionComponent
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="action">The action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public override bool Execute( RockContext rockContext, WorkflowAction action, object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();
            PDFWorkflowObject pdfWorkflowObject = new PDFWorkflowObject();
            try
            {
                pdfWorkflowObject = entity as PDFWorkflowObject;
            }
            catch
            {
                errorMessages.Add( "Could not convert entity to LavaPDFEntity" );
                return false;
            }

            using ( MemoryStream msPDF = createPDF( pdfWorkflowObject.RenderedXHTML ) )
            {
                BinaryFile pdfBinary = new BinaryFile();
                pdfBinary.Guid = Guid.NewGuid();
                pdfBinary.BinaryFileTypeId = new BinaryFileTypeService( rockContext ).Get( new Guid( Rock.SystemGuid.BinaryFiletype.DEFAULT ) ).Id;

                BinaryFileData pdfData = new BinaryFileData();
                pdfData.Content = msPDF.ToArray();

                pdfBinary.DatabaseData = pdfData;

                pdfWorkflowObject.RenderedPDF = pdfBinary;
            }

            return true;
        }
        private MemoryStream createPDF( string xhtml )
        {
            MemoryStream msOutput = new MemoryStream();
            TextReader xhtmlReader = new StringReader( xhtml );

            using ( Document document = new Document( PageSize.LETTER ) )
            {
                PdfWriter writer = PdfWriter.GetInstance( document, msOutput );
                document.Open();

                XMLWorkerHelper.GetInstance().ParseXHtml( writer, document, xhtmlReader );

                document.Close();
            }

            return msOutput;
        }

    }
}
