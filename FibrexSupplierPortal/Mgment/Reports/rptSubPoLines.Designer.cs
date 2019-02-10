namespace FibrexSupplierPortal.Mgment.Reports
{
    partial class rptSubPoLines
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrRecordID = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtblCostCodeValue = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrDescription = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell121 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTotalPrice = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTotalPrice1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTax = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPreTax = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTax = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPreTax = new DevExpress.XtraReports.UI.XRLabel();
            this.lblGrandTotal = new DevExpress.XtraReports.UI.XRLabel();
            this.PoNum = new DevExpress.XtraReports.Parameters.Parameter();
            this.dsPurchaseOrderLines1 = new FibrexSupplierPortal.Mgment.Reports.DS.dsPurchaseOrderLines();
            this.pOLINETableAdapter = new FibrexSupplierPortal.Mgment.Reports.DS.dsPurchaseOrderLinesTableAdapters.POLINETableAdapter();
            this.viewAllPurchaseOrderTableAdapter = new FibrexSupplierPortal.Mgment.Reports.DS.dsViewAllPurchaseOrderTableAdapters.ViewAllPurchaseOrderTableAdapter();
            this.Revision = new DevExpress.XtraReports.Parameters.Parameter();
            this.calculatedField1 = new DevExpress.XtraReports.UI.CalculatedField();
            this.PolineStatus = new DevExpress.XtraReports.Parameters.Parameter();
            this.po_report_spendbytopsuppliersTableAdapter = new FibrexSupplierPortal.Mgment.Reports.DS.dsSpendbyTopSuppliersTableAdapters.po_report_spendbytopsuppliersTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsPurchaseOrderLines1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.BorderColor = System.Drawing.Color.LightGray;
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Dpi = 100F;
            this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(807F, 25F);
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UsePadding = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrRecordID,
            this.xrtblCostCodeValue,
            this.xrDescription,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell12,
            this.xrTableCell121});
            this.xrTableRow1.Dpi = 100F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 11.5D;
            // 
            // xrRecordID
            // 
            this.xrRecordID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.POLINENUM")});
            this.xrRecordID.Dpi = 100F;
            this.xrRecordID.Font = new System.Drawing.Font("Arial", 8F);
            this.xrRecordID.Name = "xrRecordID";
            this.xrRecordID.StylePriority.UseFont = false;
            this.xrRecordID.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:#}";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrRecordID.Summary = xrSummary1;
            this.xrRecordID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrRecordID.Weight = 0.21543125703294136D;
            // 
            // xrtblCostCodeValue
            // 
            this.xrtblCostCodeValue.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.COSTCODE")});
            this.xrtblCostCodeValue.Dpi = 100F;
            this.xrtblCostCodeValue.Font = new System.Drawing.Font("Arial", 8F);
            this.xrtblCostCodeValue.Name = "xrtblCostCodeValue";
            this.xrtblCostCodeValue.StylePriority.UseFont = false;
            this.xrtblCostCodeValue.StylePriority.UseTextAlignment = false;
            this.xrtblCostCodeValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrtblCostCodeValue.Weight = 0.35468160102991964D;
            // 
            // xrDescription
            // 
            this.xrDescription.Dpi = 100F;
            this.xrDescription.Font = new System.Drawing.Font("Arial", 8F);
            this.xrDescription.Multiline = true;
            this.xrDescription.Name = "xrDescription";
            this.xrDescription.StylePriority.UseFont = false;
            this.xrDescription.StylePriority.UseTextAlignment = false;
            this.xrDescription.Text = "[DESCRIPTION]";
            this.xrDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrDescription.Weight = 1.0777798658109061D;
            this.xrDescription.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrDescription_BeforePrint);
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.ORDERQTY", "{0:#,###0.############################}")});
            this.xrTableCell4.Dpi = 100F;
            this.xrTableCell4.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell4.Weight = 0.44871067856819769D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.ORDERUNIT")});
            this.xrTableCell5.Dpi = 100F;
            this.xrTableCell5.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell5.Weight = 0.32307531969280656D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.UNITCOST", "{0:#,###0.############################}")});
            this.xrTableCell6.Dpi = 100F;
            this.xrTableCell6.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell6.Weight = 0.44872587838749434D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.LINECOST", "{0:#,###0.############################}")});
            this.xrTableCell12.Dpi = 100F;
            this.xrTableCell12.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell12.Weight = 0.44872590117593125D;
            // 
            // xrTableCell121
            // 
            this.xrTableCell121.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.TAXTOTAL", "{0:#,###0.############################}")});
            this.xrTableCell121.Dpi = 100F;
            this.xrTableCell121.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell121.Name = "xrTableCell121";
            this.xrTableCell121.StylePriority.UseFont = false;
            this.xrTableCell121.StylePriority.UseTextAlignment = false;
            this.xrTableCell121.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell121.Weight = 0.37685909027369136D;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 10F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 5F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel2});
            this.PageHeader.Dpi = 100F;
            this.PageHeader.HeightF = 25F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrPanel2
            // 
            this.xrPanel2.BackColor = System.Drawing.Color.LightGray;
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.xrPanel2.Dpi = 100F;
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(807F, 25F);
            this.xrPanel2.StylePriority.UseBackColor = false;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderColor = System.Drawing.Color.LightGray;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Dpi = 100F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(807.0001F, 25F);
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UsePadding = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell71,
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell91,
            this.xrTableCell10,
            this.XrTotalPrice,
            this.XrTotalPrice1});
            this.xrTableRow2.Dpi = 100F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.Dpi = 100F;
            this.xrTableCell71.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.StylePriority.UseFont = false;
            this.xrTableCell71.StylePriority.UseTextAlignment = false;
            this.xrTableCell71.Text = "Line.";
            this.xrTableCell71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell71.Weight = 0.22165191608495144D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 100F;
            this.xrTableCell7.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Cost Code";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell7.Weight = 0.36492316509869971D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 100F;
            this.xrTableCell8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Description";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell8.Weight = 1.1089012853752551D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Dpi = 100F;
            this.xrTableCell9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Quantity";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell9.Weight = 0.46166735161548278D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.Dpi = 100F;
            this.xrTableCell91.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.StylePriority.UseFont = false;
            this.xrTableCell91.StylePriority.UseTextAlignment = false;
            this.xrTableCell91.Text = "Order Unit";
            this.xrTableCell91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell91.Weight = 0.3324042562431222D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 100F;
            this.xrTableCell10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Unit Price";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell10.Weight = 0.46168303264958083D;
            // 
            // XrTotalPrice
            // 
            this.XrTotalPrice.Dpi = 100F;
            this.XrTotalPrice.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.XrTotalPrice.Name = "XrTotalPrice";
            this.XrTotalPrice.StylePriority.UseFont = false;
            this.XrTotalPrice.StylePriority.UseTextAlignment = false;
            this.XrTotalPrice.Text = "Total Price";
            this.XrTotalPrice.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.XrTotalPrice.Weight = 0.46168301404802758D;
            // 
            // XrTotalPrice1
            // 
            this.XrTotalPrice1.Dpi = 100F;
            this.XrTotalPrice1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.XrTotalPrice1.Name = "XrTotalPrice1";
            this.XrTotalPrice1.StylePriority.UseFont = false;
            this.XrTotalPrice1.StylePriority.UseTextAlignment = false;
            this.XrTotalPrice1.Text = "Tax";
            this.XrTotalPrice1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.XrTotalPrice1.Weight = 0.38774148276778336D;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
            this.ReportFooter.Dpi = 100F;
            this.ReportFooter.HeightF = 71.875F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrPanel1
            // 
            this.xrPanel1.BackColor = System.Drawing.Color.LightGray;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrTax,
            this.xrPreTax,
            this.lblTax,
            this.lblPreTax,
            this.lblGrandTotal});
            this.xrPanel1.Dpi = 100F;
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(807.0001F, 69.79166F);
            this.xrPanel1.StylePriority.UseBackColor = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BorderColor = System.Drawing.Color.White;
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.BorderWidth = 1F;
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.TOTALCOST", "{0:#,###0.############################}")});
            this.xrLabel8.Dpi = 100F;
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(626.6403F, 45.99994F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(180.3597F, 23F);
            this.xrLabel8.StylePriority.UseBorderColor = false;
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseBorderWidth = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UsePadding = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            xrSummary2.FormatString = "{0:#,#.00}";
            this.xrLabel8.Summary = xrSummary2;
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel8.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel8_BeforePrint);
            // 
            // xrTax
            // 
            this.xrTax.BorderColor = System.Drawing.Color.White;
            this.xrTax.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTax.BorderWidth = 1F;
            this.xrTax.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.TAXTOTAL")});
            this.xrTax.Dpi = 100F;
            this.xrTax.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTax.LocationFloat = new DevExpress.Utils.PointFloat(626.6403F, 22.99999F);
            this.xrTax.Name = "xrTax";
            this.xrTax.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrTax.SizeF = new System.Drawing.SizeF(180.3599F, 23F);
            this.xrTax.StylePriority.UseBorderColor = false;
            this.xrTax.StylePriority.UseBorders = false;
            this.xrTax.StylePriority.UseBorderWidth = false;
            this.xrTax.StylePriority.UseFont = false;
            this.xrTax.StylePriority.UsePadding = false;
            this.xrTax.StylePriority.UseTextAlignment = false;
            xrSummary3.FormatString = "{0:#,###0.############################}";
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTax.Summary = xrSummary3;
            this.xrTax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPreTax
            // 
            this.xrPreTax.BorderColor = System.Drawing.Color.White;
            this.xrPreTax.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrPreTax.BorderWidth = 1F;
            this.xrPreTax.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.PRETAXTOTAL", "{0:#,###0.############################}")});
            this.xrPreTax.Dpi = 100F;
            this.xrPreTax.Font = new System.Drawing.Font("Arial", 8F);
            this.xrPreTax.LocationFloat = new DevExpress.Utils.PointFloat(626.6403F, 0F);
            this.xrPreTax.Name = "xrPreTax";
            this.xrPreTax.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrPreTax.SizeF = new System.Drawing.SizeF(180.3599F, 23F);
            this.xrPreTax.StylePriority.UseBorderColor = false;
            this.xrPreTax.StylePriority.UseBorders = false;
            this.xrPreTax.StylePriority.UseBorderWidth = false;
            this.xrPreTax.StylePriority.UseFont = false;
            this.xrPreTax.StylePriority.UsePadding = false;
            this.xrPreTax.StylePriority.UseTextAlignment = false;
            this.xrPreTax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTax
            // 
            this.lblTax.BorderColor = System.Drawing.Color.White;
            this.lblTax.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblTax.BorderWidth = 1F;
            this.lblTax.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.ValTotalTax")});
            this.lblTax.Dpi = 100F;
            this.lblTax.Font = new System.Drawing.Font("Arial", 8F);
            this.lblTax.LocationFloat = new DevExpress.Utils.PointFloat(0F, 22.99999F);
            this.lblTax.Name = "lblTax";
            this.lblTax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 5, 0, 0, 100F);
            this.lblTax.SizeF = new System.Drawing.SizeF(626.6403F, 22.99999F);
            this.lblTax.StylePriority.UseBorderColor = false;
            this.lblTax.StylePriority.UseBorders = false;
            this.lblTax.StylePriority.UseBorderWidth = false;
            this.lblTax.StylePriority.UseFont = false;
            this.lblTax.StylePriority.UsePadding = false;
            this.lblTax.StylePriority.UseTextAlignment = false;
            this.lblTax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblPreTax
            // 
            this.lblPreTax.BorderColor = System.Drawing.Color.White;
            this.lblPreTax.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblPreTax.BorderWidth = 1F;
            this.lblPreTax.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.ValPretax")});
            this.lblPreTax.Dpi = 100F;
            this.lblPreTax.Font = new System.Drawing.Font("Arial", 8F);
            this.lblPreTax.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblPreTax.Name = "lblPreTax";
            this.lblPreTax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 5, 0, 0, 100F);
            this.lblPreTax.SizeF = new System.Drawing.SizeF(626.6403F, 23F);
            this.lblPreTax.StylePriority.UseBorderColor = false;
            this.lblPreTax.StylePriority.UseBorders = false;
            this.lblPreTax.StylePriority.UseBorderWidth = false;
            this.lblPreTax.StylePriority.UseFont = false;
            this.lblPreTax.StylePriority.UsePadding = false;
            this.lblPreTax.StylePriority.UseTextAlignment = false;
            this.lblPreTax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.BorderColor = System.Drawing.Color.White;
            this.lblGrandTotal.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.lblGrandTotal.BorderWidth = 1F;
            this.lblGrandTotal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "POLINE.ValGrandtax")});
            this.lblGrandTotal.Dpi = 100F;
            this.lblGrandTotal.Font = new System.Drawing.Font("Arial", 8F);
            this.lblGrandTotal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 45.99997F);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 5, 0, 0, 100F);
            this.lblGrandTotal.SizeF = new System.Drawing.SizeF(626.6403F, 23F);
            this.lblGrandTotal.StylePriority.UseBorderColor = false;
            this.lblGrandTotal.StylePriority.UseBorders = false;
            this.lblGrandTotal.StylePriority.UseBorderWidth = false;
            this.lblGrandTotal.StylePriority.UseFont = false;
            this.lblGrandTotal.StylePriority.UsePadding = false;
            this.lblGrandTotal.StylePriority.UseTextAlignment = false;
            this.lblGrandTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // PoNum
            // 
            this.PoNum.Description = "PoNum";
            this.PoNum.Name = "PoNum";
            this.PoNum.Type = typeof(int);
            this.PoNum.ValueInfo = "0";
            // 
            // dsPurchaseOrderLines1
            // 
            this.dsPurchaseOrderLines1.DataSetName = "dsPurchaseOrderLines";
            this.dsPurchaseOrderLines1.EnforceConstraints = false;
            this.dsPurchaseOrderLines1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pOLINETableAdapter
            // 
            this.pOLINETableAdapter.ClearBeforeFill = true;
            // 
            // viewAllPurchaseOrderTableAdapter
            // 
            this.viewAllPurchaseOrderTableAdapter.ClearBeforeFill = true;
            // 
            // Revision
            // 
            this.Revision.Name = "Revision";
            this.Revision.Type = typeof(short);
            this.Revision.ValueInfo = "0";
            // 
            // calculatedField1
            // 
            this.calculatedField1.DataMember = "POLINE";
            this.calculatedField1.Expression = "[DESCRIPTION] +\r\n\r\n[MODELNUM]+ [MANUFACUTRER]\r\n\r\n+[REMARK]";
            this.calculatedField1.Name = "calculatedField1";
            // 
            // PolineStatus
            // 
            this.PolineStatus.Description = "Parameter1";
            this.PolineStatus.Name = "PolineStatus";
            // 
            // po_report_spendbytopsuppliersTableAdapter
            // 
            this.po_report_spendbytopsuppliersTableAdapter.ClearBeforeFill = true;
            // 
            // rptSubPoLines
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.ReportFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.calculatedField1});
            this.DataAdapter = this.pOLINETableAdapter;
            this.DataMember = "POLINE";
            this.DataSource = this.dsPurchaseOrderLines1;
            this.FilterString = "[PONUM] = ?PoNum And [POREVISION] = ?Revision";
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(17, 10, 10, 5);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.PoNum,
            this.Revision,
            this.PolineStatus});
            this.Version = "16.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.rptSubPoLines_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsPurchaseOrderLines1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRPanel xrPanel2;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell71;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell91;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell XrTotalPrice;
        private DevExpress.XtraReports.UI.XRTableCell XrTotalPrice1; 
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrRecordID;
        private DevExpress.XtraReports.UI.XRTableCell xrtblCostCodeValue;
        private DevExpress.XtraReports.UI.XRTableCell xrDescription;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.Parameters.Parameter PoNum;
        private DS.dsPurchaseOrderLines dsPurchaseOrderLines1;
        private DS.dsPurchaseOrderLinesTableAdapters.POLINETableAdapter pOLINETableAdapter;
        private DS.dsViewAllPurchaseOrderTableAdapters.ViewAllPurchaseOrderTableAdapter viewAllPurchaseOrderTableAdapter;
        private DevExpress.XtraReports.Parameters.Parameter Revision;
        private DevExpress.XtraReports.UI.XRPanel xrPanel1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell121;
        private DevExpress.XtraReports.UI.XRLabel lblPreTax;
        private DevExpress.XtraReports.UI.XRLabel xrPreTax;
        private DevExpress.XtraReports.UI.XRLabel lblTax;
        private DevExpress.XtraReports.UI.XRLabel xrTax;
        private DevExpress.XtraReports.UI.XRLabel lblGrandTotal;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.CalculatedField calculatedField1;
        private DevExpress.XtraReports.Parameters.Parameter PolineStatus;
        private DS.dsSpendbyTopSuppliersTableAdapters.po_report_spendbytopsuppliersTableAdapter po_report_spendbytopsuppliersTableAdapter;
    }
}
