Imports System.ComponentModel
Imports VGM2MID.Util

Namespace Core
    Namespace MMap
        ' Used For Memory/Port Mapping
        Public MustInherit Class MemoryMapper
            Protected Mapper() As MapperUnit

            ' Mapper Unit
            Protected Structure MapperUnit
                Dim IsValid As Boolean
                ' Mapper Unit ID
                Dim hMapperID As Long

                Dim StartAddress As Integer
                Dim IncreAddress As Integer     ' IncreAddress = 0: No Block (Such As FNum, etc...)

                Dim StartBit As Integer
                Dim CountBit As Integer

                ' Some Data Exceeding 8-Bits
                Dim fHasExtra As Boolean
                Dim ExtraAddress As Integer
                Dim ExtraStartBit As Integer
                Dim ExtraCountBit As Integer

                Public Sub New(valid As Boolean, staddr As Integer, incaddr As Integer, stbit As Integer, cntbit As Integer, extra As Boolean, extraddr As Integer, extrastbit As Integer, extracntbit As Integer, hID As Long)
                    IsValid = valid : hMapperID = hID : StartAddress = staddr : IncreAddress = incaddr : StartBit = stbit : CountBit = cntbit : fHasExtra = extra : ExtraAddress = extraddr : ExtraStartBit = extrastbit : ExtraCountBit = extracntbit
                End Sub

            End Structure

            Private Function FindMapperByID(hID As Long) As MapperUnit
                For i As Integer = 0 To Mapper.Length
                    If (hID = Mapper(i).hMapperID) Then
                        Return Mapper(i)
                    End If
                Next
                Return Nothing
            End Function

            Public ReadOnly Property Addr(hID As Long, extra As Boolean) As Integer
                Get
                    Dim map As MapperUnit = FindMapperByID(hID)
                    If (map.fHasExtra And extra) Then Return map.ExtraAddress
                    Return map.StartAddress
                End Get
            End Property
            Public ReadOnly Property Addr(hID As Long, id As Integer, blk As Integer, extra As Boolean) As Integer
                Get
                    Dim map As MapperUnit = FindMapperByID(hID)
                    If (map.fHasExtra And extra) Then Return map.ExtraAddress + id + map.IncreAddress * blk
                    Return map.StartAddress + id + map.IncreAddress * blk
                End Get
            End Property

            Public ReadOnly Property Extra(hID As Long) As Integer
                Get
                    Dim map As MapperUnit = FindMapperByID(hID)
                    Return map.fHasExtra
                End Get
            End Property

            Public ReadOnly Property Shift(hID As Long, extra As Boolean) As Integer
                Get
                    Dim map As MapperUnit = FindMapperByID(hID)
                    If (map.fHasExtra And extra) Then Return map.ExtraStartBit
                    Return map.StartBit
                End Get
            End Property

            Public ReadOnly Property Mask(hID As Long, extra As Boolean) As Integer
                Get
                    Dim map As MapperUnit = FindMapperByID(hID)
                    If (map.fHasExtra And extra) Then Return (1 << map.ExtraCountBit) - 1
                    Return (1 << map.CountBit) - 1
                End Get
            End Property


        End Class
    End Namespace
End Namespace