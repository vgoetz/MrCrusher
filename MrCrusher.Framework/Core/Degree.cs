using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace MrCrusher.Framework.Core {

    [Serializable]
    public enum Quadrant {
        // ReSharper disable InconsistentNaming
        [EnumMember]
        I   = 1,
        [EnumMember]
        II  = 2,
        [EnumMember]
        III = 3,
        [EnumMember]
        IV  = 4
        // ReSharper restore InconsistentNaming
    }

    [Serializable]
    public class Degree {

        private double _value;
        private const double EqualyTolerance = 0.1;

        public Degree(double value) {
            Value = value;
        }

        public double Value {
            get { return _value; }
            set {
                if (value >= 0) {
                    _value = value % 360;
                } else {
                    var positiveValue = Math.Abs(value) % 360;
                    _value = (360 - positiveValue) % 360;
                }
            }
        }

        public Quadrant Quadrant {
            get {
                if (_value >= 0 && _value < 90) {
                    return Quadrant.I;
                } 
                if (_value >= 90 && _value < 180) {
                    return Quadrant.II;
                }
                if (_value >= 180 && _value < 270)
                {
                    return Quadrant.III;
                }
                
                return Quadrant.IV;
            }
        }

        public static Degree Default {
            get { return 0; }
        }

        public static implicit operator double(Degree d) {
            return d.Value;
        }

        public static implicit operator Degree(double d) {
            return new Degree(d);
        }

        public static Degree operator +(Degree a, Degree b) {
            return new Degree((a._value + b._value) % 360);
        }
        
        public static Degree operator -(Degree a, Degree b) {
            return new Degree((a._value - b._value) % 360);
        }

        public static bool operator ==(Degree a, Degree b) {
            if (ReferenceEquals(a, b)) {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) {
                return false;
            }

            return Math.Abs(a._value - b._value) < EqualyTolerance;
        }

        public static bool operator !=(Degree a, Degree b) {
            return !(a == b);
        }
        
        // Equals should be consistent with operator ==.
        public override bool Equals(Object obj) {
            var other = obj as Degree;
            if (other == null) {
                return false;
            }
            
            return Math.Abs(_value - other._value) < EqualyTolerance;
        }

        public bool Equals(Degree other) {
            if (other == null) {
                return false;
            }

            return (Math.Abs(_value - other._value) < EqualyTolerance);
        }

        public override int GetHashCode() {
            return _value.GetHashCode();
        }

        public override string ToString() {
            return string.Format("{0:0.00}",_value);
        }
    }
}
