package org.cqrs101.invoices.repositories;

import javax.inject.Inject;
import javax.inject.Named;

import org.cqrs101.BaseRepository;
import org.cqrs101.RepositoryHelper;

import java.util.UUID;

@Named("invoicesRepository")
public class InvoicesRepository extends BaseRepository<Invoice> {

    @Inject
    public InvoicesRepository(RepositoryHelper helper) {
        super(helper);
    }

    @Override
    public Invoice save(Invoice item) {
        return (Invoice) helper.save(item,
                (obj, id) -> ((Invoice) obj).setId(id),
                (obj) -> ((Invoice) obj).getId());
    }
}
