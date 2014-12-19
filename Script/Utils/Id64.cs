using UnityEngine;
using System.Collections;

[System.Serializable]
public class Id64
{
	public int low;
	public int high;
	
	public Id64(long l) {
		low = (int)(l & 0xFFFFFFFF);
		high = (int)(l >> 32);
	}
	
	public static implicit operator long(Id64 l) {
		long h = ((long)l.high) << 32;
		long lo = l.low & 0x00000000FFFFFFFF;		
		return h | lo;
	}
	
	public static implicit operator Id64(long l) {
		return new Id64(l);
	}
	
	public override bool Equals(System.Object b) {
		long la = this;
		long lb = ((Id64)b);
		return la.Equals(lb);
	}
	
	public override int GetHashCode() {
		long l = (long)this;
		return l.GetHashCode();
	}
}