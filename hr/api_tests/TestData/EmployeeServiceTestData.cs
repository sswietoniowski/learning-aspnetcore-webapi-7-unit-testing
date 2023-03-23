using System.Collections;

namespace Hr.Api.Tests.TestData;

public class EmployeeServiceTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 100, true };
        yield return new object[] { 200, false };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}