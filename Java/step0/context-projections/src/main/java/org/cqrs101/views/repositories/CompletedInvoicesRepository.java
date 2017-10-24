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
public class CompletedInvoicesRepository extends BaseRepository<CompletedInvoice> {

    @Inject
    public CompletedInvoicesRepository(RepositoryHelper helper) {
        super(helper);
    }

    @Override
    public CompletedInvoice save(CompletedInvoice item) {
        return (CompletedInvoice) helper.save(item,
                (obj, id) -> ((CompletedInvoice) obj).setId(id),
                (obj) -> ((CompletedInvoice) obj).getId());
    }
}
