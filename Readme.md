<!-- default file list -->
*Files to look at*:

* [MainWindow.xaml](./CS/WpfApplication1/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/WpfApplication1/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/WpfApplication1/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/WpfApplication1/MainWindow.xaml.vb))
<!-- default file list end -->
# How to customize appointment tooltips


<p>The following example demonstrates how to customize the visual representation of appointment tooltips, including showing resource images within appointment tooltips.</p>


<h3>Description</h3>

<p>To show resource images within tooltips displayed for appointments, you should do the following:<br />
1. Handle the <strong>AppointmentViewInfoCustomizing</strong> event.<br />
2. In the event handler, assign the resource images to the <strong>CustomViewInfo</strong> property of the <strong>AppointmentViewInfo</strong> object, which is accessible via <strong>AppointmentViewInfoCustomizingEventArgs.ViewInfo</strong>.<br />
3. In XAML, define a custom template for the appointment tooltips, bind the Image object to the <strong>CustomViewInfo</strong> property and assign the template to the <strong>AppointmentToolTipContentTemplate</strong> property.</p>

<br/>


