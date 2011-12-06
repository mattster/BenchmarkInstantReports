<%@ Page Title="Student Grades" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="StudentStats1.aspx.cs" Inherits="Benchmark_Instant_Reports_2.StudentStats" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tcol1
        {
            width: 100px;
            text-align: right;
            height: 50px;
        }
        .tcol2
        {
            width: 410px;
            height: 50px;
        }
        .tcol3
        {
            width: 250px;
        }
        
        .tcolCustom1
        {
            width: 500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Student Grades</h1>
    <p>
        The Student Statistics Report shows individual student grades for a specific test,
        teacher and period.<br />
    </p>
    <br />
    <table>
        <tr>
            <td class="tcol1">
                Select Campus:
            </td>
            <td class="tcol2">
                <asp:Panel ID="pnlCampus" runat="server">
                    <asp:UpdatePanel ID="updpnlCampus" class="CampusUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddCampus" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddCampus_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tcol1">
                <asp:Panel ID="pnlTestFilter" CssClass="FilterPanel" runat="server">
                    <asp:UpdatePanel ID="updpnlTestFilter" class="FilterPanel" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="FilterHdrCol1">
                                        <div class="popupH1">
                                            Filter Tests By</div>
                                    </td>
                                    <td class="FilterHdrCol2">
                                        <asp:ImageButton ID="CloseTestFilterImg" runat="server" CssClass="PopupCloseImg, floatright"
                                            OnClientClick="hidePCE()" AlternateText="Close Popup" ImageUrl="~/content/images/icon_close_window.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterHdrCol1">
                                        <span style="font-weight: bold;">Test List is instantly updated. Click outside popup
                                            to continue.</span>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="FilterCol1">
                                        <div class="popupLabel">
                                            Curriculum Area:</div>
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:DropDownList ID="ddTFCur" CssClass="popupDDL" runat="server" Height="28px" Width="150px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddTFCur_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterCol1">
                                        <div class="popupLabel">
                                            Subject:</div>
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:DropDownList ID="ddTFSubj" CssClass="popupDDL" runat="server" Height="28px"
                                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddTFSubj_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterCol1">
                                        <div class="popupLabel">
                                            Test Type:</div>
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:DropDownList ID="ddTFTestType" CssClass="popupDDL" runat="server" Height="28px"
                                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddTFTestType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterCol1">
                                        <div class="popupLabel">
                                            Test Version:</div>
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:DropDownList ID="ddTFTestVersion" CssClass="popupDDL" runat="server" Height="28px"
                                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddTFTestVersion_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterCol1">
                                        &nbsp;<br />
                                        &nbsp;
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:Button ID="btnTFReset" CssClass="popupButton" runat="server" Text="Reset" OnClick="btnTFReset_Click"
                                            OnClientClick="hidePCE()" UseSubmitBehavior="false" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <br />
                <asp:UpdatePanel ID="updpnlImgFilterTests" runat="server">
                    <ContentTemplate>
                        <asp:Image CssClass="filterImg, floatleft" ID="imgFilterTests" runat="server" AlternateText="Filter Tests"
                            ImageUrl="~/content/images/f-circ-20x20.png" />
                        <ajaxToolkit:PopupControlExtender ID="pceFilterTests" TargetControlID="imgFilterTests"
                            BehaviorID="popupCE" PopupControlID="updpnlTestFilter" Position="Top" OffsetY="-50"
                            OffsetX="550" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblSelectTest" CssClass="" runat="server">Select Test:</asp:Label>
                <br />
                &nbsp;<br />
                &nbsp;
            </td>
            <td class="tcol2">
                <asp:Panel ID="pnlBenchmark" runat="server">
                    <asp:UpdatePanel ID="updpnlBenchmark" class="BenchmarkUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="updpnlFilteredTestsLabel" class="DDLabelAbove" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="listTests" runat="server" Height="28px" Width="400px" OnSelectedIndexChanged="listTests_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                    &nbsp;
                                    <asp:Label ID="lblTestsFiltered" CssClass="DDLabelAboveText" Visible="false" runat="server">Test List is Filtered. Click Filter Button to change.</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tcol1">
                Select Teacher:
            </td>
            <td class="tcol2">
                <asp:Panel ID="pnlTeacher" runat="server">
                    <asp:UpdatePanel ID="updpnlTeacher" class="TeacherUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddTeacher" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddTeacher_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol3">
                <asp:UpdateProgress ID="progressTeachers" runat="server" AssociatedUpdatePanelID="updpnlBenchmark">
                    <ProgressTemplate>
                        <asp:Image ID="imgTeacherProgress" runat="server" CssClass="imgLoader, floatleft"
                            AlternateText="loading..." ImageUrl="~/content/images/ajax-loader2.gif" />
                        <div class="popupH1">
                            Loading List of Teachers...</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
        <tr>
            <td class="tcol1">
                &nbsp;
            </td>
            <td class="tcol2">
                <asp:Panel ID="pnlNoScanData" runat="server">
                    <asp:UpdatePanel ID="updpnlNoScanData" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblNoScanData" runat="server" Text="There is no scanned data currently available for this test at this campus"
                                ForeColor="#0000FF"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="tcol1">
                <asp:Panel ID="pnlGenReport" runat="server">
                    <asp:UpdatePanel ID="updpnlGenReport" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" OnClick="btnGenReport_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol2">
                <asp:UpdateProgress ID="progressGenReport" runat="server" AssociatedUpdatePanelID="updpnlGenReport">
                    <ProgressTemplate>
                        <asp:Image ID="imgReportProgress" runat="server" CssClass="imgLoader, floatleft"
                            AlternateText="loading..." ImageUrl="~/content/images/ajax-loader1.gif" />
                        <div class="popupH1">
                            Generating Report ...</div>
                        <asp:Label ID="lblReportProgress" runat="server"></asp:Label></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Panel ID="pnlReportViewer" runat="server">
        <asp:UpdatePanel ID="updpnlReportViewer" runat="server">
            <ContentTemplate>
                <rsweb:ReportViewer ID="repvwStudentStats2a" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="800px" Height="113px" SizeToReportContent="True">
                    <LocalReport ReportPath="StudentStatsRep2a.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetStudentStatsRep2" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
                    TypeName="Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter"
                    DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}"
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
