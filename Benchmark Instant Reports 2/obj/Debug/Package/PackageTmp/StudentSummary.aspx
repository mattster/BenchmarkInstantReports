<%@ Page Title="BIR: Student Summaries" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="StudentSummary.aspx.cs" Inherits="Benchmark_Instant_Reports_2.WebForm4" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            width: 240px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Student Grade Summaries</h1>
    <p>
        The Student Summary Report shows student test scores and a question-by-question
        view of their test answers. This report is designed to be cut into small sections
        and given to each student.</p>
    <table style="width: 95%;">
        <tr>
            <td class="style13" valign="middle">
                <table style="width: 97%;">
                    <tr>
                        <td class="style13" valign="middle" align="right">
                            Select Campus:
                        </td>
                        <td align="left" class="style2" valign="middle">
                            <asp:DropDownList ID="ddCampus" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddCampus_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style13" valign="middle" align="right">
                            <asp:Label ID="lblEnterSchoolPassword" runat="server" Font-Bold="True" ForeColor="#3366FF"
                                Text="Enter school password:"></asp:Label>
                        </td>
                        <td align="left" class="style2" valign="middle">
                            <asp:TextBox ID="txtbxSchoolPassword" runat="server" Width="68px" TextMode="Password"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnEnterPassword" runat="server" OnClick="btnEnterPassword_Click"
                                Text="Submit" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style13" valign="middle" align="right">
                            Select Test:
                        </td>
                        <td align="left" class="style2" valign="middle">
                            <asp:DropDownList ID="ddBenchmark" runat="server" Height="28px" Width="445px" OnSelectedIndexChanged="ddBenchmark_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style13" valign="middle" align="right">
                            &nbsp;
                        </td>
                        <td align="left" class="style2" valign="middle">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style13" valign="middle" align="right">
                            Select Teacher:
                        </td>
                        <td align="left" class="style2" valign="middle">
                            <asp:DropDownList ID="ddTeacher" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddTeacher_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="middle" class="style18">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style13" valign="middle">
                &nbsp;
                <asp:Label ID="lblNoScanData" runat="server" Text="There is no scanned data currently available for this test at this campus"
                    ForeColor="#0000FF"></asp:Label>
            </td>
            </strong>
            <td valign="middle" class="style17">
                **ALL class periods are automatically selected.
            </td>
        </tr>
        <strong __designer:mapid="189">
            <tr>
                <td class="style13" align="center" valign="middle">
                    <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" OnClick="btnGenReport_Click" />
                </td>
                <td valign="middle" class="style18">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style13" align="center" valign="middle">
                    <asp:Label ID="lblAlignmentNote" runat="server" Font-Bold="False" ForeColor="#FF3300"
                        Text="** Report is optimally formatted for printing from Internet Explorer and may not appear to be aligned correctly on screen"></asp:Label>
                </td>
                <td valign="middle" class="style18">
                    &nbsp;
                </td>
            </tr>
    </table>
    <asp:HyperLink Target="_blank" Text="" ID="modalpopupcontrol" runat="server"></asp:HyperLink>
    <asp:Panel ID="popupPanel" runat="server" CssClass="popup-dialog">
        <div id="dialogContents">
            Incorrect school password. Please try again.<br />
            <asp:Button ID="ButtonOK" runat="server" Text="OK" /></div>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="mpupIncorrectPassword" runat="server" TargetControlID="modalpopupcontrol"
        PopupControlID="popupPanel" OkControlID="ButtonOK" DropShadow="true" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>
    <rsweb:ReportViewer ID="repvwStudentSummary" runat="server" Font-Names="Verdana"
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" SizeToReportContent="True"
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport ReportPath="StudentSummaryRep.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetStudentStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByUseFilter"
        TypeName="Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter"
        UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="Original_STUDENT_ID" Type="String" />
            <asp:Parameter Name="Original_TEST_ID" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="STUDENT_ID" Type="String" />
            <asp:Parameter Name="STUDENT_NAME" Type="String" />
            <asp:Parameter Name="TEST_ID" Type="String" />
            <asp:Parameter Name="SCAN_DATETIME" Type="DateTime" />
            <asp:Parameter Name="LETTER_GRADE" Type="String" />
            <asp:Parameter Name="NUM_CORRECT" Type="Decimal" />
            <asp:Parameter Name="NUM_TOTAL" Type="Decimal" />
            <asp:Parameter Name="PCT_CORRECT" Type="Decimal" />
            <asp:Parameter Name="CAMPUS" Type="String" />
            <asp:Parameter Name="TEACHER" Type="String" />
            <asp:Parameter Name="PERIOD" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="STUDENT_NAME" Type="String" />
            <asp:Parameter Name="SCAN_DATETIME" Type="DateTime" />
            <asp:Parameter Name="LETTER_GRADE" Type="String" />
            <asp:Parameter Name="NUM_CORRECT" Type="Decimal" />
            <asp:Parameter Name="NUM_TOTAL" Type="Decimal" />
            <asp:Parameter Name="PCT_CORRECT" Type="Decimal" />
            <asp:Parameter Name="CAMPUS" Type="String" />
            <asp:Parameter Name="TEACHER" Type="String" />
            <asp:Parameter Name="PERIOD" Type="String" />
            <asp:Parameter Name="Original_STUDENT_ID" Type="String" />
            <asp:Parameter Name="Original_TEST_ID" Type="String" />
        </UpdateParameters>
    </asp:ObjectDataSource>
</asp:Content>
