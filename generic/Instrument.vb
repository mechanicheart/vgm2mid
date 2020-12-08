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

    Public Class FMInstrument : Inherits Instrument
        Public Structure stFMInstrOp
            Public iAttrAR As Integer
            Public iAttrD1R As Integer
            Public iAttrD2R As Integer
            Public iAttrSR As Integer
            Public iAttrRR As Integer

            Public iAttrD1L As Integer
            Public iAttrD2L As Integer
            Public iAttrTL As Integer
            Public iAttrSL As Integer
        End Structure

        Public stAttr() As stFMInstrOp

        Public Property AR(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrAR
            End Get
            Set(val As Integer)
                stAttr(id).iAttrAR = val
            End Set
        End Property

        Public Property D1R(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrD1R
            End Get
            Set(val As Integer)
                stAttr(id).iAttrD1R = val
            End Set
        End Property

        Public Property D2R(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrD2R
            End Get
            Set(val As Integer)
                stAttr(id).iAttrD2R = val
            End Set
        End Property

        Public Property SR(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrSR
            End Get
            Set(val As Integer)
                stAttr(id).iAttrSR = val
            End Set
        End Property

        Public Property RR(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrRR
            End Get
            Set(val As Integer)
                stAttr(id).iAttrRR = val
            End Set
        End Property

        Public Property D1L(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrD1L
            End Get
            Set(val As Integer)
                stAttr(id).iAttrD1L = val
            End Set
        End Property

        Public Property D2L(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrD2L
            End Get
            Set(val As Integer)
                stAttr(id).iAttrD2L = val
            End Set
        End Property

        Public Property TL(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrTL
            End Get
            Set(val As Integer)
                stAttr(id).iAttrTL = val
            End Set
        End Property

        Public Property SL(id As Integer) As Integer
            Get
                Return stAttr(id).iAttrSL
            End Get
            Set(val As Integer)
                stAttr(id).iAttrSL = val
            End Set
        End Property

        Public Overrides Function CompareTo(instr As Instrument) As Boolean
            Return True
        End Function

    End Class

    Public Class PSGInstrument : Inherits Instrument
        Public Overrides Function CompareTo(instr As Instrument) As Boolean
            Return True
        End Function
    End Class

End Namespace
