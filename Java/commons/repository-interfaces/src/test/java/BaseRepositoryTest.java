import org.cqrs101.BaseRepository;
import org.cqrs101.RepositoryHelper;
import org.junit.Before;
import org.junit.Test;

import java.util.List;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;

public class BaseRepositoryTest {
    private RepositoryHelper repoHelper;

    @Before
    public void setUp(){
        repoHelper = mock(RepositoryHelper.class);
    }

    @Test
    public void constructorShouldCreateTheRepositoryHelper(){
        BaseRepository<String> target = new BaseRepository<String>(repoHelper) {
            @Override
            public String save(String item) {
                return null;
            }
        };
        verify(repoHelper, times(1)).create(String.class);
    }
}
