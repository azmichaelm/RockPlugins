﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BreakoutGroupAttendanceSummary.ascx.cs" Inherits="RockWeb.Blocks.Reporting.Children.BreakoutGroupAttendanceSummary" %>

<asp:UpdatePanel ID="upReport" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-sm-4">
                <Rock:GradePicker runat="server" ID="gpGrade" Label="Grades" />
            </div>
            <div class="col-sm-4">
                <Rock:DateRangePicker runat="server" ID="drpRange" Label="Attendance Range" />
            </div>
            <div class="col-sm-12">
                <asp:LinkButton runat="server" ID="btnGenerate" CssClass="btn btn-primary"  OnClick="btnGenerate_Click">Generate</asp:LinkButton>
            </div>
        </div>
    </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnGenerate" />
    </Triggers>
</asp:UpdatePanel>
