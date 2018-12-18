
namespace Giveth
{
	public class TestInfo
	{
		protected bool Equals(TestInfo other)
		{
			return string.Equals(TestName, other.TestName) && string.Equals(TestClassName, other.TestClassName) && string.Equals(TestMethodName, other.TestMethodName);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((TestInfo) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (TestName != null ? TestName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (TestClassName != null ? TestClassName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (TestMethodName != null ? TestMethodName.GetHashCode() : 0);
				return hashCode;
			}
		}
		public string TestName { get; set; }
		public string TestClassName { get; set; }
		public string TestMethodName { get; set; }
	}
}