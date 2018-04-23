Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data
Imports System.Windows
Imports System.Data.OleDb
Imports DevExpress.Xpf.Core.Native
Imports System.Collections.Generic
Imports System.Windows.Media.Imaging
Imports DevExpress.XtraScheduler
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.Xpf.Scheduler.Drawing

Namespace WpfApplication1
	Partial Public Class MainWindow
		Inherits Window
		Private resourceImages As New Dictionary(Of Resource, BitmapImage)()

		Private dataSet As CarsDBDataSet
		Private adapter As CarsDBDataSetTableAdapters.CarSchedulingTableAdapter

		Public Sub New()
			InitializeComponent()

			schedulerControl1.Start = New System.DateTime(2010, 7, 11, 0, 0, 0, 0)

			Me.dataSet = New CarsDBDataSet()

			' Bind Scheduler storage to appointment data
			Me.schedulerControl1.Storage.AppointmentStorage.DataSource = dataSet.CarScheduling

			' Load data into the 'CarsDBDataSet.CarScheduling' table. 
			Me.adapter = New CarsDBDataSetTableAdapters.CarSchedulingTableAdapter()
			Me.adapter.Fill(dataSet.CarScheduling)

			' Bind Scheduler storage to resource data
			Me.schedulerControl1.Storage.ResourceStorage.DataSource = dataSet.Cars

			' Load data into the 'CarsDBDataSet.Cars' table.
			Dim carsAdapter As New CarsDBDataSetTableAdapters.CarsTableAdapter()
			carsAdapter.Fill(dataSet.Cars)

			AddHandler schedulerControl1.Storage.AppointmentsInserted, AddressOf Storage_AppointmentsModified
			AddHandler schedulerControl1.Storage.AppointmentsChanged, AddressOf Storage_AppointmentsModified
			AddHandler schedulerControl1.Storage.AppointmentsDeleted, AddressOf Storage_AppointmentsModified

			AddHandler adapter.Adapter.RowUpdated, AddressOf adapter_RowUpdated
		End Sub

		Private Sub Storage_AppointmentsModified(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
			Me.adapter.Adapter.Update(Me.dataSet)
			Me.dataSet.AcceptChanges()

		End Sub

		Private Sub adapter_RowUpdated(ByVal sender As Object, ByVal e As System.Data.OleDb.OleDbRowUpdatedEventArgs)
			If e.Status = UpdateStatus.Continue AndAlso e.StatementType = StatementType.Insert Then
				Dim id As Integer = 0
				Using cmd As New OleDbCommand("SELECT @@IDENTITY", adapter.Connection)
					id = CInt(Fix(cmd.ExecuteScalar()))
				End Using
				e.Row("ID") = id
			End If
		End Sub

		Private Sub schedulerControl1_AppointmentViewInfoCustomizing(ByVal sender As Object, ByVal e As AppointmentViewInfoCustomizingEventArgs)
			Dim viewInfo As AppointmentViewInfo = e.ViewInfo
			Dim resource As Resource = schedulerControl1.Storage.ResourceStorage.GetResourceById(viewInfo.Appointment.ResourceId)
			If resource Is Resource.Empty OrElse resource.Image Is Nothing Then
				viewInfo.CustomViewInfo = Nothing
			Else
				If (Not Me.resourceImages.ContainsKey(resource)) Then
					Me.resourceImages(resource) = ImageHelper.CreateImageFromStream(ConvertImageToMemoryStream(resource.Image))
				End If

				viewInfo.CustomViewInfo = Me.resourceImages(resource)
			End If
		End Sub

		Public Shared Function ConvertImageToMemoryStream(ByVal img As System.Drawing.Image) As MemoryStream
			Dim ms = New MemoryStream()
			img.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
			Return ms
		End Function

	End Class
End Namespace
