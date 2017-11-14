package org.cqrs101.views.repositories;

import javax.inject.Inject;
import javax.inject.Named;

import org.cqrs101.BaseRepository;
import org.cqrs101.RepositoryHelper;

@Named("completedInvoicesRepository")
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
