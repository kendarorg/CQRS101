package org.cqrs101.views.repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Inject;
import javax.inject.Named;

import org.cqrs101.BaseRepository;
import org.cqrs101.Repository;
import org.cqrs101.RepositoryHelper;

@Named("inProgressInvoicesRepository")
public class InProgressInvoicesRepository extends BaseRepository<InProgressInvoice> {

    @Inject
    public InProgressInvoicesRepository(RepositoryHelper helper) {
        super(helper);
    }

    @Override
    public InProgressInvoice save(InProgressInvoice item) {
        return (InProgressInvoice) helper.save(item,
                (obj, id) -> ((InProgressInvoice) obj).setId(id),
                (obj) -> ((InProgressInvoice) obj).getId());
    }
}
