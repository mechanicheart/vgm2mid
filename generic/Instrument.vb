Namespace Instrument
    Public MustInherit Class Instrument
        Private szInstrName As String
        Public Property Name() As String
            Get
                Return szInstrName
            End Get
            Set(val As String)
                szInstrName = val
            End Set
        End Property

        Public MustOverride Function CompareTo(instr As Instrument) As Boolean
    End Class

    Public Class FMInstrument
        Inherits Instrument
        Public Structure stFMInstrAttr
            Private iAttrAR As Integer
            Private iAttrD1R As Integer
            Private iAttrD2R As Integer
            Private iAttrSR As Integer
            Private iAttrRR As Integer

            Private iAttrD1L As Integer
            Private iAttrD2L As Integer
            Private iAttrTL As Integer
            Private iAttrSL As Integer

            Public Property AR() As Integer
                Get
                    Return iAttrAR
                End Get
                Set(val As Integer)
                    iAttrAR = val
                End Set
            End Property

            Public Property D1R() As Integer
                Get
                    Return iAttrD1R
                End Get
                Set(val As Integer)
                    iAttrD1R = val
                End Set
            End Property

            Public Property D2R() As Integer
                Get
                    Return iAttrD2R
                End Get
                Set(val As Integer)
                    iAttrD2R = val
                End Set
            End Property

            Public Property SR() As Integer
                Get
                    Return iAttrSR
                End Get
                Set(val As Integer)
                    iAttrSR = val
                End Set
            End Property

            Public Property RR() As Integer
                Get
                    Return iAttrRR
                End Get
                Set(val As Integer)
                    iAttrRR = val
                End Set
            End Property

            Public Property D1L() As Integer
                Get
                    Return iAttrD1L
                End Get
                Set(val As Integer)
                    iAttrD1L = val
                End Set
            End Property

            Public Property D2L() As Integer
                Get
                    Return iAttrD2L
                End Get
                Set(val As Integer)
                    iAttrD2L = val
                End Set
            End Property

            Public Property TL() As Integer
                Get
                    Return iAttrTL
                End Get
                Set(val As Integer)
                    iAttrTL = val
                End Set
            End Property

            Public Property SL() As Integer
                Get
                    Return iAttrSL
                End Get
                Set(val As Integer)
                    iAttrSL = val
                End Set
            End Property

        End Structure

        Private stAttr As stFMInstrAttr = New stFMInstrAttr

        Public ReadOnly Property Attr() As stFMInstrAttr
            Get
                Return stAttr
            End Get
        End Property

        Public Overrides Function CompareTo(instr As Instrument) As Boolean


            Return True
        End Function

    End Class

End Namespace
