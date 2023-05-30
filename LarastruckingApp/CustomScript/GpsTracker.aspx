<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GpsTracker.aspx.cs" Inherits="GpsTracker"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>Laras Gps | Dart Innovations</title>
<link rel="ICON" href="media/favicon.png" sizes="16x16 32x32 48x48 64x64" type="image/vnd.microsoft.icon" />
</head>
<body>
	<div id="authentication" class="custombg">
		<form id="form1" runat="server">
			<div style="padding-bottom:15px;">
				<asp:Label id="GpsAccessToken" runat="server" ></asp:Label>
				<asp:Label id="GpsRefreshToken" runat="server" ></asp:Label>
			</div>
			<div style="padding-bottom:15px;">
				<p>VehicleLive List</p>
				<asp:Label id="VehicleListLive" runat="server" ></asp:Label>
				
			</div>
			<div  style="padding-bottom:15px;">
					<p>VehicleList</p>
				<asp:Label id="VehicleList" runat="server" ></asp:Label>
			</div>
			
		</form>
	</div>
</body>
</html>