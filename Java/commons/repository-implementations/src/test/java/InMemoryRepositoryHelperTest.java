import org.cqrs101.InMemoryRepositoryHelper;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;

import java.util.List;
import java.util.UUID;

import static org.junit.Assert.*;

public class InMemoryRepositoryHelperTest {

    private InMemoryRepositoryHelper factory;

    @Before
    public void setUp(){
        factory = new InMemoryRepositoryHelper();
    }

    @After
    public void cleanUp(){
        factory.truncate();
    }

    @Test
    public void shouldAssignTheNameBasedOnClassName(){

        InMemoryRepositoryHelper result =(InMemoryRepositoryHelper) factory.create(Object.class);

        assertEquals("OBJECT", result.getName());
    }

    @Test
    public void shouldCreateDifferentInstancesForEachCall(){
        InMemoryRepositoryHelper target = new InMemoryRepositoryHelper();

        InMemoryRepositoryHelper result1 = (InMemoryRepositoryHelper) target.create(SimpleObject.class);
        InMemoryRepositoryHelper result2 = (InMemoryRepositoryHelper) target.create(SimpleObject.class);

        assertNotSame(result1,result2);
    }

    @Test
    public void shouldStoreAndRetrieveTheSameObject(){
        InMemoryRepositoryHelper target = (InMemoryRepositoryHelper) factory.create(SimpleObject.class);
        SimpleObject so = new SimpleObject();
        UUID id = UUID.randomUUID();
        so.setId(id);
        so.setData("");

        target.save(so,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );

        SimpleObject result = (SimpleObject)target.getById(id);
        assertSame(so,result);
    }


    @Test
    public void shouldStoreAndRetrieveTheSameObjectBetweenTwoHelpers(){
        InMemoryRepositoryHelper targetWrite = (InMemoryRepositoryHelper) factory.create(SimpleObject.class);
        InMemoryRepositoryHelper targetRead =(InMemoryRepositoryHelper) factory.create(SimpleObject.class);
        SimpleObject so = new SimpleObject();
        UUID id = UUID.randomUUID();
        so.setId(id);
        so.setData("");

        targetWrite.save(so,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );

        SimpleObject result = (SimpleObject)targetRead.getById(id);
        assertSame(so,result);
    }

    @Test
    public void shouldAssignIdWhenNotPresent(){
        InMemoryRepositoryHelper target =(InMemoryRepositoryHelper) factory.create(SimpleObject.class);
        SimpleObject so = new SimpleObject();

        assertNull(so.getId());

        target.save(so,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );

        List<Object> resultList = target.getAll();
        assertEquals(1,resultList.size());
        SimpleObject result = (SimpleObject)resultList.get(0);
        assertNotNull(result.getId());
    }

    @Test
    public void shouldRetrieveAllItems(){
        InMemoryRepositoryHelper target =(InMemoryRepositoryHelper) factory.create(SimpleObject.class);
        SimpleObject so1 = new SimpleObject();
        SimpleObject so2 = new SimpleObject();

        target.save(so1,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );
        target.save(so2,
                (o, uuid) -> ((SimpleObject)o).setId(uuid),
                o ->((SimpleObject)o).getId() );

        List<Object> resultList = target.getAll();
        assertEquals(2,resultList.size());
        SimpleObject result1 = (SimpleObject)resultList.get(0);
        SimpleObject result2 = (SimpleObject)resultList.get(1);
        assertNotEquals(result1.getId(),result2.getId());
    }
}
