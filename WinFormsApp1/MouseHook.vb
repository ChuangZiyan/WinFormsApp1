﻿Imports System.Net.NetworkInformation
Imports System.Reflection, System.Threading, System.ComponentModel, System.Runtime.InteropServices

Public Class MouseHook
    Private Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Integer, ByVal lpfn As MouseProcDelegate, ByVal hmod As IntPtr, ByVal dwThreadId As Integer) As Integer
    Private Declare Function CallNextHookEx Lib "user32" (ByVal hHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByVal lParam As MSLLHOOKSTRUCT) As Integer
    Private Declare Function UnhookWindowsHookEx Lib "user32" (ByVal hHook As Integer) As Integer
    Private Delegate Function MouseProcDelegate(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Integer

    Private Structure MSLLHOOKSTRUCT
        Public pt As Point
        Public mouseData As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure

    Public Enum Wheel_Direction
        WheelUp
        WheelDown
    End Enum

    Private Const HC_ACTION As Integer = 0
    Private Const WH_MOUSE_LL As Integer = 14
    Private Const WM_MOUSEMOVE As Integer = &H200
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_LBUTTONUP As Integer = &H202
    Private Const WM_RBUTTONDOWN As Integer = &H204
    Private Const WM_RBUTTONUP As Integer = &H205
    Private Const WM_MBUTTONDOWN As Integer = &H207
    Private Const WM_MBUTTONUP As Integer = &H208
    Private Const WM_MOUSEWHEEL As Integer = &H20A

    'Private MouseHook As Integer
    Private MouseHook As IntPtr = IntPtr.Zero
    Private MouseHookDelegate As MouseProcDelegate

    Public Event Mouse_Move(ByVal e As Point)
    Public Event Mouse_Left_Down(ByVal e As Point)
    Public Event Mouse_Left_Up(ByVal e As Point)
    Public Event Mouse_Right_Down(ByVal e As Point)
    Public Event Mouse_Right_Up(ByVal e As Point)
    Public Event Mouse_Middle_Down(ByVal e As Point)
    Public Event Mouse_Middle_Up(ByVal e As Point)
    Public Event Mouse_Wheel(ByVal e As Point, ByVal Direction As Wheel_Direction)

    Public Sub New()
        MouseHookDelegate = New MouseProcDelegate(AddressOf MouseProc)
        MouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookDelegate, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt64, 0)

    End Sub

    Private Function MouseProc(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Integer

        If (nCode = HC_ACTION) Then
            Select Case wParam
                Case WM_MOUSEMOVE
                    RaiseEvent Mouse_Move(lParam.pt)
                Case WM_LBUTTONDOWN
                    RaiseEvent Mouse_Left_Down(lParam.pt)
                Case WM_LBUTTONUP
                    RaiseEvent Mouse_Left_Up(lParam.pt)
                Case WM_RBUTTONDOWN
                    RaiseEvent Mouse_Right_Down(lParam.pt)
                Case WM_RBUTTONUP
                    RaiseEvent Mouse_Right_Up(lParam.pt)
                Case WM_MBUTTONDOWN
                    RaiseEvent Mouse_Middle_Down(lParam.pt)
                Case WM_MBUTTONUP
                    RaiseEvent Mouse_Middle_Up(lParam.pt)
                Case WM_MOUSEWHEEL
                    Dim wDirection As Wheel_Direction
                    If lParam.mouseData < 0 Then
                        wDirection = Wheel_Direction.WheelDown
                    Else
                        wDirection = Wheel_Direction.WheelUp
                    End If
                    RaiseEvent Mouse_Wheel(lParam.pt, wDirection)
            End Select
        End If
        Return CallNextHookEx(MouseHook, nCode, wParam, lParam)
    End Function

    Protected Overrides Sub Finalize()
        UnhookWindowsHookEx(MouseHook)
        MyBase.Finalize()
    End Sub
End Class
